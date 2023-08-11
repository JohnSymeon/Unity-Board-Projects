/*
    The connect four board class providing the board, the player's and computer's turn
    and an AI that uses the monte carlo method to find the best move by determining which
    play brings the biggest wins to losses ratio.
*/
public class Board_Grid
{
    private int width;
    private int height;
    public Cell[,] board;
    public Vector3 last_played_position;
    public static float x_offset = 0f;
    public static float y_offset = 0f;

    public bool MODE_Tetris;

    public Board_Grid(int height, int width)
    {
        this.width=width;
        this.height = height;
        board = new Cell[height,width];
        for(int i=0;i<board.GetLength(0);i++)
        {
            for(int j=0;j<board.GetLength(1);j++)
            {
                board[i,j] = new Cell();
                board[i,j].SetCoordinate(i,j);

            }
        }
    }
    //Used by player to play a tile
    public bool Who_Plays_And_Return_If_Full(Cell_status who, int col)
    {
        for(int i=0;i<board.GetLength(0);i++)
        {
            if(board[i,col].status==Cell_status.Neutral)
            {
                board[i,col].status = who;
                last_played_position = board[i,col].cell_obj.transform.position;

                if(i==height-1)
                    return true;
                else
                    return false;
            }  
        }
        return false;
    }
    //Used by computer to play a tile
    public bool Who_Plays_And_Return_If_Full(Cell_status who, int col, Cell[,] board )
    {
        for(int i=0;i<height;i++)
        {
            if(board[i,col].status==Cell_status.Neutral)
            {
                board[i,col].status = who;
                return true;
            }
        }
        return false;
    }

    public void AI_Plays(Cell_status who, int MONTE_NUMBER)
    {
        int[] check_for_wins = new int[width];
        int[] check_for_losses = new int[width];
        int[] arr1 = {0,1,2,3,4,5};
        int[] arr2 = {0,1,2,3,4,5,6};
        for(int i=0;i<width;i++)
        {
            check_for_wins[i]=0;
            check_for_losses[i]=0;
        }
            
        

        for(int k=0;k<MONTE_NUMBER;k++)
        {
            Cell[,] board_monte = new Cell[height, width];

            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    board_monte[i,j] = new Cell();
                    board_monte[i,j].status = board[i,j].status;
                }
            }

            System.Random rnd = new System.Random();
            arr1 = arr1.OrderBy(x=>rnd.Next()).ToArray();
            System.Random rnd2 = new System.Random();
            arr2 = arr2.OrderBy(x=>rnd2.Next()).ToArray();
            
            int[] comp_played = new int[width];
            for(int i=0;i<width;i++)
                comp_played[i] = -1;

            bool exited = false;
            foreach(int i in arr1)
            {
                foreach(int j in arr2)
                {
                    if(board_monte[height-1,j].status==Cell_status.Neutral)
                    {
                        if(Random.Range(0f,1f)>=0.5f)
                        {
                            Who_Plays_And_Return_If_Full(who,j,board_monte);
                            if(Check_for_Victory(who, board_monte))
                            {
                                exited = true;
                                break;
                            }
                        }
                        else
                        {
                            Who_Plays_And_Return_If_Full(Cell_status.Player,j,board_monte);
                            if(Check_for_Victory(Cell_status.Player, board_monte))
                            {
                                exited = true;
                                break;
                            }
                        }  
                    }   
                }
                if(exited)
                    break;
            }
            
            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    if(board[i,j].status==Cell_status.Neutral && board_monte[i,j].status==Cell_status.Computer && comp_played[j]==-1 && 
                    ( i==0 || board[i-1,j].status!=Cell_status.Neutral ) )
                    {
                        comp_played[j] = i;
                    }
                }
            }

    
            if(Check_for_Victory(Cell_status.Player, board_monte))
            {
                for(int i=0;i<width;i++)
                {
                    if(comp_played[i]>-1)
                        check_for_losses[i]+=1;
                }
                
            }
            else if(Check_for_Victory(who, board_monte))
            {
                for(int i=0;i<width;i++)
                {
                    if(comp_played[i]>-1)
                        check_for_wins[i]+=1;
                }

            }
        }
       
        float max =0f;
        int pos=0;
        for(int i=0;i<width;i++)
        {
            if(( (float)check_for_wins[i] )/( (float)check_for_losses[i] )>max)
            {
                pos = i;
                max = ( (float)check_for_wins[i] )/( (float)check_for_losses[i] );
            }
        }

        Debug.Log(max);
        Who_Plays_And_Return_If_Full(who,pos);
    }


    //initialization of objects
    public void Set_Object_to_Cell(int i, int j, GameObject obj)
    {
        board[i,j].cell_obj = obj;
    }

    //check if anybody wins at board
    public bool Check_for_Victory(Cell_status who)
    {
        for(int i=0;i<board.GetLength(0);i++)
        {
            for(int j=0; j<board.GetLength(1);j++)
            {
                if(board[i,j].status==who && Check_Four(i,j,who,board))
                    return true;
            }
        }
        return false;
    }

    //check if anybody wins at the monte carlo board
    public bool Check_for_Victory(Cell_status who, Cell[,] game_board)
    {
        for(int i=0;i<game_board.GetLength(0);i++)
        {
            for(int j=0; j<game_board.GetLength(1);j++)
            {
                if(game_board[i,j].status==who && Check_Four(i,j,who,game_board))
                    return true;
            }
        }
        return false;
    }

    
    //method used to determine victor
    private bool Check_Four(int i, int j, Cell_status who, Cell[,] board)
    {
        //vertical check
        if(i<height-3 &&
        board[i+1,j].status==who && board[i+2,j].status==who && board[i+3,j].status==who)
        {
            if(MODE_Tetris)
                MODE_Tetris_method(board[i,j], board[i+1,j], board[i+2,j], board[i+3,j]);
            return true;
        }
            
        //horizontal check
        if(j<width-3 &&
        board[i,j+1].status==who && board[i,j+2].status==who && board[i,j+3].status==who)
        {
            if(MODE_Tetris)
                MODE_Tetris_method(board[i,j], board[i,j+1], board[i,j+2], board[i,j+3]);
            return true;
        }

        //diagonal check upper
        if(i<height-3 && j<width-3 &&
        board[i+1,j+1].status==who &&  board[i+2,j+2].status==who && board[i+3,j+3].status==who)
        {
            if(MODE_Tetris)
                MODE_Tetris_method(board[i,j], board[i+1,j+1], board[i+2,j+2], board[i+3,j+3]);
            return true;
        }
        //diagonal check lower
        if(i>2 && j<width-3 &&
        board[i-1,j+1].status==who &&  board[i-2,j+2].status==who && board[i-3,j+3].status==who)
        {
            if(MODE_Tetris)
                MODE_Tetris_method(board[i,j], board[i-1,j+1], board[i-2,j+2], board[i-3,j+3]);
            return true;
        }
        return false;
    }

    private void MODE_Tetris_method(Cell c1, Cell c2, Cell c3, Cell c4)
    {
        Cell[] cells = {c1,c2,c3,c4};

        for(int i=0;i<4;i++)
        {
            cells[i].status = Cell_status.Neutral;
            cells[i].kill_switch = true;
            var k = cells[i].x_pos;
            var j = cells[i].y_pos;
            if(k<height-1 && board[k+1,j].status!=Cell_status.Neutral)
            {
                board[k+1,j].status = Cell_status.Neutral;
                board[k+1,j].kill_switch = true;
            }
            if(k>1 && board[k-1,j].status != Cell_status.Neutral)
            {
                    board[k-1,j].status = Cell_status.Neutral;
                    board[k-1,j].kill_switch = true;;
            }
            if(j<width-1 && board[k,j+1].status != Cell_status.Neutral)
            {
                board[k,j+1].status = Cell_status.Neutral;
                board[k,j+1].kill_switch = true;
            }
            if(j>1 && board[k,j-1].status != Cell_status.Neutral)
            {
                board[k,j-1].status = Cell_status.Neutral;
                board[k,j-1].kill_switch = true;
            }
                
        }

        for(int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                var bot = Find_bottom_of_col(i,j);
                if( bot!= i )
                {
                    board[bot,j].status = board[i,j].status;
                    board[i,j].status = Cell_status.Neutral;
                }
            }
        }
    }

    public int Find_bottom_of_col(int curr_height, int col)
    {
        for(int j=curr_height;j>0;j--)
        {
            if(board[j, col].status!=Cell_status.Neutral)
            {
                return j;
            }
        }
        return 0;
    }


}
